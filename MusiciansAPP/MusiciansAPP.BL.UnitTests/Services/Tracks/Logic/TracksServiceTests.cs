using AutoMapper;
using Moq;
using MusiciansAPP.BL.Services.Tracks.BLModels;
using MusiciansAPP.BL.Services.Tracks.Logic;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.UnitTests.Services.Tracks.Logic;

[TestFixture]
public class TracksServiceTests
{
    private const int PageSize = 2;
    private const int Page = 1;

    private const string TracksArtistName = "a";
    private const string TrackOneName = "aa";
    private const int TrackOnePlayCount = 1;
    private const string TrackTwoName = "bb";
    private const int TrackTwoPlayCount = 2;

    private TracksService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Track> _tracks;

    [SetUp]
    public void SetUp()
    {
        _webDataProviderMock = new Mock<IWebDataProvider>();

        var artistTraksDAL = new ArtistTracksDAL
        {
            ArtistName = TracksArtistName,
            Tracks = new List<TrackDAL>
            {
                new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
                new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount }
            }
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistTopTracksAsync(
                TracksArtistName, PageSize, Page)).ReturnsAsync(artistTraksDAL);

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _unitOfWorkMock = new Mock<IUnitOfWork>();

        _tracks = new List<Track>
        {
            new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
            new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount }
        };
        _unitOfWorkMock.Setup(uow => uow.Tracks.GetTopTracksForArtistAsync(
                TracksArtistName, PageSize, Page)).ReturnsAsync(() => _tracks);

        _unitOfWorkMock.Setup(uow => uow.Artists.GetArtistDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(new Artist());

        _service = new TracksService(
            _webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        //Arrange
        _tracks = _tracks.Take(amount).ToList();

        //Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        //Arrange
        _tracks = _tracks.Take(amount).ToList();

        //Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopTracksAsync_DBHasTracksAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        //Arrange
        _tracks = _tracks.Take(amount).ToList();

        //Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Tracks.AddOrUpdateArtistTracksAsync(It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Domain.Track>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_FullDataReturned()
    {
        //Arrange
        _tracks[0].PlayCount = null;

        //Act
        var result = await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopTracksAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_WebServiceWasCalled()
    {
        //Arrange
        _tracks[0].PlayCount = null;

        //Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page), Times.Once);
    }

    [Test]
    public async Task GetArtistTopTracksAsync_DBDoesNotHaveFullTrackData_UnitOfWorkSavingWasCalled()
    {
        //Arrange
        _tracks[0].PlayCount = null;

        //Act
        await _service.GetArtistTopTracksAsync(TracksArtistName, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Tracks.AddOrUpdateArtistTracksAsync(It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Domain.Track>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    private void VerifyTopTracksAreCorrect(IEnumerable<TrackBL> model)
    {
        var tracks = model.ToList();

        Assert.That(tracks[0].Name, Is.EqualTo(TrackOneName));
        Assert.That(tracks[0].PlayCount, Is.EqualTo(TrackOnePlayCount));

        Assert.That(tracks[1].Name, Is.EqualTo(TrackTwoName));
        Assert.That(tracks[1].PlayCount, Is.EqualTo(TrackTwoPlayCount));
    }
}