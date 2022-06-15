using AutoMapper;
using Moq;
using MusiciansAPP.BL.ArtistsService.BLModels;
using MusiciansAPP.DAL.DALModels;
using MusiciansAPP.DAL.DBDataProvider.Interfaces;
using MusiciansAPP.DAL.WebDataProvider.Interfaces;
using MusiciansAPP.Domain;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusiciansAPP.BL.UnitTests.ArtistsService.Logic;

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

    private BL.ArtistsService.Logic.ArtistsService _service;
    private Mock<IWebDataProvider> _webDataProvider;
    private Mock<IUnitOfWork> _unitOfWork;
    private IMapper _mapper;
    private List<Artist> _artists;
    private ArtistsPagingDAL _artistsPaging;

    [SetUp]
    public void SetUp()
    {
        _artistsPaging = new ArtistsPagingDAL
        {
            Artists = new List<ArtistDAL>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
            },
            PagingData = new PagingDataDAL { TotalItems = TotalItems }
        };
        _webDataProvider = new Mock<IWebDataProvider>();
        _webDataProvider.Setup(wdp => wdp.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(() => _artistsPaging);

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _artists = new List<Artist>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
        };
        _unitOfWork = new Mock<IUnitOfWork>();
        _unitOfWork.Setup(uow => uow.Artists.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(() => _artists);

        _service = new BL.ArtistsService.Logic.ArtistsService(
            _webDataProvider.Object, _mapper, _unitOfWork.Object);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_FullDataReturned()
    {
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
        Assert.NotZero(result.PagingData.TotalItems);
    }

    [Test]
    public async Task GetTopArtistsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        await _service.GetTopArtistsAsync(PageSize, Page);

        _webDataProvider.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_FullDataReturned(int amount)
    {
        _artists = _artists.Take(amount).ToList();

        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
        Assert.That(result.PagingData.TotalItems, Is.EqualTo(TotalItems));
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_WebServiceWasCalled(int amount)
    {
        _artists = _artists.Take(amount).ToList();

        await _service.GetTopArtistsAsync(PageSize, Page);

        _webDataProvider.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullData_UnitOfWorkSavingWasCalled(
        int amount)
    {
        _artists = _artists.Take(amount).ToList();

        await _service.GetTopArtistsAsync(PageSize, Page);

        _unitOfWork.Verify(
            u => u.Artists.AddOrUpdateRangeAsync(It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
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