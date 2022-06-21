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

    private const string ArtistDetailsName = "c";
    private const string ArtistDetailsImageUrl = "cc";
    private const string ArtistDetailsBiography = "ccc";

    private const string ArtistNameForSimilar = "d";

    private ArtistsService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Artist> _artists;
    private Artist _artistDetails;
    private Artist _artistWithSimilar;

    [SetUp]
    public void SetUp()
    {
        _webDataProviderMock = new Mock<IWebDataProvider>();
        var artistsPaging = new ArtistsPagingDAL
        {
            Artists = new List<ArtistDAL>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
            },
            PagingData = new PagingDataDAL { TotalItems = TotalItems }
        };
        _webDataProviderMock.Setup(wdp => wdp.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(artistsPaging);

        var artistDetailsDAL = new ArtistDetailsDAL
        {
            Name = ArtistDetailsName,
            ImageUrl = ArtistDetailsImageUrl,
            Biography = ArtistDetailsBiography,
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistDetailsAsync(ArtistDetailsName))
            .ReturnsAsync(artistDetailsDAL);

        var similarArtistsDAL = new SimilarArtistsDAL
        {
            ArtistName = ArtistNameForSimilar,
            Artists = new List<ArtistDAL>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
            }
        };
        _webDataProviderMock.Setup(wdp => wdp.GetSimilarArtistsAsync(
            ArtistNameForSimilar, PageSize, Page)).ReturnsAsync(similarArtistsDAL);

        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddMaps("MusiciansAPP.BL")));

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _artists = new List<Artist>
        {
            new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
            new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
        };
        _unitOfWorkMock.Setup(uow => uow.Artists.GetTopArtistsAsync(PageSize, Page))
            .ReturnsAsync(() => _artists);

        _artistDetails = new Artist
        {
            Name = ArtistDetailsName,
            ImageUrl = ArtistDetailsImageUrl,
            Biography = ArtistDetailsBiography
        };
        _unitOfWorkMock.Setup(uow => uow.Artists.GetArtistDetailsAsync(It.IsAny<string>()))
            .ReturnsAsync(() => _artistDetails);

        _artistWithSimilar = new Artist
        {
            Name = ArtistNameForSimilar,
            SimilarArtists = new List<Artist>
            {
                new() { Name = ArtistOneName, ImageUrl = ArtistOneImageUrl },
                new() { Name = ArtistTwoName, ImageUrl = ArtistTwoImageUrl }
            }
        };
        _unitOfWorkMock.Setup(uow => uow.Artists.GetArtistWithSimilarAsync(
            ArtistNameForSimilar, PageSize, Page)).ReturnsAsync(() => _artistWithSimilar);

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
        VerifyTopArtistsAreCorrect(result.Artists);
        Assert.Greater(result.PagingData.TotalItems, 0);
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
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        //Arrange
        _artists = _artists.Take(amount).ToList();

        //Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result.Artists);
        Assert.That(result.PagingData.TotalItems, Is.EqualTo(TotalItems));
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
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
    public async Task GetTopArtistsAsync_DBHasArtistsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
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

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_FullDataReturned(
        string imageUrl)
    {
        //Arrange
        _artists[0].ImageUrl = imageUrl;

        //Act
        var result = await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        Assert.That(result.Artists.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result.Artists);
        Assert.That(result.PagingData.TotalItems, Is.EqualTo(TotalItems));
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_WebServiceWasCalled(
        string imageUrl)
    {
        //Arrange
        _artists[0].ImageUrl = imageUrl;

        //Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetTopArtistsAsync(PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetTopArtistsAsync_DBDoesNotHaveFullArtistData_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        //Arrange
        _artists[0].ImageUrl = imageUrl;

        //Act
        await _service.GetTopArtistsAsync(PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateRangeAsync(It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistDetailsAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistDetailsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_FullDataReturned(
        string imageUrl)
    {
        //Arrange
        _artistDetails.ImageUrl = imageUrl;

        //Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        //Arrange
        _artistDetails.ImageUrl = imageUrl;

        //Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        //Arrange
        _artistDetails.ImageUrl = imageUrl;

        //Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateAsync(It.IsAny<Artist>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_FullDataReturned(
        string biography)
    {
        //Arrange
        _artistDetails.Biography = biography;

        //Act
        var result = await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        VerifyArtistDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_WebServiceWasCalled(
        string biography)
    {
        //Arrange
        _artistDetails.Biography = biography;

        //Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistDetailsAsync(ArtistDetailsName), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistDetailsAsync_DBDoesNotHaveBiography_UnitOfWorkSavingWasCalled(
        string biography)
    {
        //Arrange
        _artistDetails.Biography = biography;

        //Act
        await _service.GetArtistDetailsAsync(ArtistDetailsName);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateAsync(It.IsAny<Artist>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetSimilarArtistsAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [Test]
    public async Task GetSimilarArtistsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        //Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        //Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetSimilarArtistsAsync_DBHasSimilarArtistsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists = _artistWithSimilar.SimilarArtists.Take(amount).ToList();

        //Act
        await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateSimilarArtistsAsync(It.IsAny<string>(),
                It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_FullDataReturned(
        string imageUrl)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        //Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopArtistsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        //Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetSimilarArtistsAsync_DBDoesNotHaveSimilarArtistImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        //Arrange
        _artistWithSimilar.SimilarArtists[0].ImageUrl = imageUrl;

        //Act
        var result = await _service.GetSimilarArtistsAsync(ArtistNameForSimilar, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Artists.AddOrUpdateSimilarArtistsAsync(It.IsAny<string>(),
                It.IsAny<IEnumerable<Artist>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    private void VerifyTopArtistsAreCorrect(IEnumerable<ArtistBL> model)
    {
        var artists = model.ToList();
        Assert.That(artists[0].Name, Is.EqualTo(ArtistOneName));
        Assert.That(artists[0].ImageUrl, Is.EqualTo(ArtistOneImageUrl));

        Assert.That(artists[1].Name, Is.EqualTo(ArtistTwoName));
        Assert.That(artists[1].ImageUrl, Is.EqualTo(ArtistTwoImageUrl));
    }

    private void VerifyArtistDetailsAreCorrect(ArtistDetailsBL artist)
    {
        Assert.That(artist.Name, Is.EqualTo(ArtistDetailsName));
        Assert.That(artist.ImageUrl, Is.EqualTo(ArtistDetailsImageUrl));
        Assert.That(artist.Biography, Is.EqualTo(ArtistDetailsBiography));
    }
}