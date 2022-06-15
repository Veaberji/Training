using AutoMapper;
using Moq;
using MusiciansAPP.BL.Services.Artists.BLModels;
using MusiciansAPP.BL.Services.Artists.Logic;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.UnitTests.Services.Artists.Logic;

[TestFixture]
public class ArtistsServiceTests
{
    private const int TotalItems = 3;
    private const int PageSize = 2;
    private const int Page = 1;
    private const string ArtistOneName = "a";
    private const string ArtistOneImageUrl = "aa";
    private const string ArtistTwoName = "b";
    private const string ArtistTwoImageUrl = "bb";

    private ArtistsService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Artist> _artists;

    [SetUp]
    public void SetUp()
    {
        var artistsPaging = new ArtistsPagingDAL
        {
            Artists = new List<ArtistDAL>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
            },
            PagingData = new PagingDataDAL { TotalItems = TotalItems }
        };
        _webDataProviderMock = new Mock<IWebDataProvider>();
        _webDataProviderMock.Setup(wdp => wdp.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(() => artistsPaging);

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _artists = new List<Artist>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
        };
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(uow => uow.Artists.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(() => _artists);

        _service = new ArtistsService(
            _webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
        Assert.NotZero(result.PagingData.TotalItems);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_FullDataReturned(int amount)
    {
        //Arrange
        _artists = _artists.Take(amount).ToList();

        //Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
        Assert.That(result.PagingData.TotalItems, Is.EqualTo(TotalItems));
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_WebServiceWasCalled(int amount)
    {
        //Arrange
        _artists = _artists.Take(amount).ToList();

        //Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_UnitOfWorkSavingWasCalled(
        int amount)
    {
        //Arrange
        _artists = _artists.Take(amount).ToList();

        //Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateRangeAsync(It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    private void VerifyTopArtistsAreCorrect(ArtistsPagingBL model)
    {
        var artists = model.Artists.ToList();
        Assert.That(artists[0].Name, Is.EqualTo(ArtistOneName));
        Assert.That(artists[0].ImageUrl, Is.EqualTo(ArtistOneImageUrl));

        Assert.That(artists[1].Name, Is.EqualTo(ArtistTwoName));
        Assert.That(artists[1].ImageUrl, Is.EqualTo(ArtistTwoImageUrl));
    }
}