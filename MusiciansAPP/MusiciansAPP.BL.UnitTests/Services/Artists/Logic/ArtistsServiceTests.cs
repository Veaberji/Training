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

    private const string TracksArtistName = "a";
    private const string TrackOneName = "aa";
    private const int TrackOnePlayCount = 1;
    private const string TrackTwoName = "bb";
    private const int TrackTwoPlayCount = 2;

    private const string AlbumsArtistName = "a";
    private const string AlbumOneName = "aa";
    private const int AlbumOnePlayCount = 1;
    private const string AlbumOneImageUrl = "aaa";
    private const string AlbumTwoName = "bb";
    private const int AlbumTwoPlayCount = 2;
    private const string AlbumTwoImageUrl = "bbb";

    private const string AlbumDetailsArtistName = "c";
    private const string AlbumDetailsName = "cc";
    private const string AlbumDetailsImageUrl = "ccc";
    private const string AlbumTrackOneName = "dd";
    private const int AlbumTrackOneDurationInSeconds = 1;
    private const string AlbumTrackTwoName = "ee";
    private const int AlbumTrackTwoDurationInSeconds = 2;

    private ArtistsService _service;
    private Mock<IWebDataProvider> _webDataProviderMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private IMapper _mapper;
    private List<Artist> _artists;
    private Artist _artistDetails;
    private List<Track> _tracks;
    private List<Album> _albums;
    private Artist _artistWithSimilar;
    private Album _albumDetails;

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

        var artistAlbumsDAL = new ArtistAlbumsDAL
        {
            ArtistName = TracksArtistName,
            Albums = new List<AlbumDAL>
            {
                new() { Name = AlbumOneName, PlayCount = AlbumOnePlayCount, ImageUrl = AlbumOneImageUrl},
                new() { Name = AlbumTwoName, PlayCount = AlbumTwoPlayCount, ImageUrl = AlbumTwoImageUrl}
            }
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistTopAlbumsAsync(
            AlbumsArtistName, PageSize, Page)).ReturnsAsync(artistAlbumsDAL);

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

        var albumDetails = new AlbumDetailsDAL()
        {
            Name = AlbumDetailsName,
            ArtistName = AlbumDetailsArtistName,
            ImageUrl = AlbumDetailsImageUrl,
            Tracks = new List<AlbumTrackDAL>
            {
                new(){ Name = AlbumTrackOneName, DurationInSeconds = AlbumTrackOneDurationInSeconds },
                new(){ Name = AlbumTrackTwoName, DurationInSeconds = AlbumTrackTwoDurationInSeconds }
            }
        };
        _webDataProviderMock.Setup(wdp => wdp.GetArtistAlbumDetailsAsync(
            AlbumDetailsArtistName, AlbumDetailsName)).ReturnsAsync(albumDetails);

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

        _tracks = new List<Track>
        {
            new() { Name = TrackOneName, PlayCount = TrackOnePlayCount },
            new() { Name = TrackTwoName, PlayCount = TrackTwoPlayCount }
        };
        _unitOfWorkMock.Setup(uow => uow.Tracks.GetTopTracksForArtistAsync(
                TracksArtistName, PageSize, Page)).ReturnsAsync(() => _tracks);

        _albums = new List<Album>
        {
            new() { Name = AlbumOneName, PlayCount = AlbumOnePlayCount, ImageUrl = AlbumOneImageUrl},
            new() { Name = AlbumTwoName, PlayCount = AlbumTwoPlayCount, ImageUrl = AlbumTwoImageUrl}
        };
        _unitOfWorkMock.Setup(uow => uow.Albums.GetTopAlbumsForArtistAsync(
            AlbumsArtistName, PageSize, Page)).ReturnsAsync(() => _albums);

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

        _albumDetails = new Album
        {
            Name = AlbumDetailsName,
            Artist = new Artist { Name = AlbumDetailsArtistName },
            ImageUrl = AlbumDetailsImageUrl,
            Tracks = new List<Track>
            {
                new(){ Name = AlbumTrackOneName, DurationInSeconds = AlbumTrackOneDurationInSeconds },
                new(){ Name = AlbumTrackTwoName, DurationInSeconds = AlbumTrackTwoDurationInSeconds }
            }
        };
        _unitOfWorkMock.Setup(uow => uow.Albums.GetAlbumDetailsAsync(
            AlbumDetailsArtistName, AlbumDetailsName)).ReturnsAsync(() => _albumDetails);

        _service = new ArtistsService(
            _webDataProviderMock.Object, _mapper, _unitOfWorkMock.Object);
    }

    #region TopArtists
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
    #endregion

    #region ArtistDetails
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
    #endregion

    #region TopTracks

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
                It.IsAny<IEnumerable<Track>>()), Times.Once);
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
                It.IsAny<IEnumerable<Track>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }
    #endregion

    #region TopAlbums

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Never);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_FullDataReturned(
        int amount)
    {
        //Arrange
        _albums = _albums.Take(amount).ToList();

        //Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_WebServiceWasCalled(
        int amount)
    {
        //Arrange
        _albums = _albums.Take(amount).ToList();

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(0)]
    [TestCase(PageSize - 1)]
    public async Task GetArtistTopAlbumsAsync_DBHasAlbumsAmountLessThanPageSize_UnitOfWorkSavingWasCalled(
        int amount)
    {
        //Arrange
        _albums = _albums.Take(amount).ToList();

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_FullDataReturned(
        string imageUrl)
    {
        //Arrange
        _albums[0].ImageUrl = imageUrl;

        //Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        //Arrange
        _albums[0].ImageUrl = imageUrl;

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        //Arrange
        _albums[0].ImageUrl = imageUrl;

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumPlayCount_FullDataReturned()
    {
        //Arrange
        _albums[0].PlayCount = null;

        //Act
        var result = await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        Assert.That(result.Count(), Is.EqualTo(PageSize));
        VerifyTopAlbumsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled()
    {
        //Arrange
        _albums[0].PlayCount = null;

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page), Times.Once);
    }

    [Test]
    public async Task GetArtistTopAlbumsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled()
    {
        //Arrange
        _albums[0].PlayCount = null;

        //Act
        await _service.GetArtistTopAlbumsAsync(AlbumsArtistName, PageSize, Page);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.Albums.AddOrUpdateArtistAlbumsAsync(It.IsAny<Artist>(),
                It.IsAny<IEnumerable<Album>>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
    }

    #endregion

    #region SimilarArtists

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
    #endregion




    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBHasFullData_FullDataReturned()
    {
        //Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBHasFullData_WebServiceWasNotCalled()
    {
        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Never);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_FullDataReturned(
        string imageUrl)
    {
        //Arrange
        _albumDetails.ImageUrl = imageUrl;

        //Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_WebServiceWasCalled(
        string imageUrl)
    {
        //Arrange
        _albumDetails.ImageUrl = imageUrl;

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumImageUrl_UnitOfWorkSavingWasCalled(
        string imageUrl)
    {
        //Arrange
        _albumDetails.ImageUrl = imageUrl;

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_FullDataReturned()
    {
        //Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        //Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_WebServiceWasCalled()
    {
        //Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveTrackDurationInSeconds_UnitOfWorkSavingWasCalled()
    {
        //Arrange
        _albumDetails.Tracks[0].DurationInSeconds = null;

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_FullDataReturned()
    {
        //Arrange
        _albumDetails.Tracks = new List<Track>();

        //Act
        var result = await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        VerifyAlbumDetailsAreCorrect(result);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_WebServiceWasCalled()
    {
        //Arrange
        _albumDetails.Tracks = new List<Track>();

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _webDataProviderMock.Verify(
            w => w.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName),
            Times.Once);
    }

    [Test]
    public async Task GetArtistAlbumDetailsAsync_DBDoesNotHaveAlbumTracks_UnitOfWorkSavingWasCalled()
    {
        //Arrange
        _albumDetails.Tracks = new List<Track>();

        //Act
        await _service.GetArtistAlbumDetailsAsync(AlbumDetailsArtistName, AlbumDetailsName);

        //Assert
        _unitOfWorkMock.Verify(
            u => u.SaveAlbumDetailsAsync(It.IsAny<Album>(), It.IsAny<IEnumerable<Track>>()),
            Times.Once);
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

    private void VerifyTopTracksAreCorrect(IEnumerable<TrackBL> model)
    {
        var tracks = model.ToList();

        Assert.That(tracks[0].Name, Is.EqualTo(TrackOneName));
        Assert.That(tracks[0].PlayCount, Is.EqualTo(TrackOnePlayCount));

        Assert.That(tracks[1].Name, Is.EqualTo(TrackTwoName));
        Assert.That(tracks[1].PlayCount, Is.EqualTo(TrackTwoPlayCount));
    }

    private void VerifyTopAlbumsAreCorrect(IEnumerable<AlbumBL> model)
    {
        var albums = model.ToList();

        Assert.That(albums[0].Name, Is.EqualTo(AlbumOneName));
        Assert.That(albums[0].PlayCount, Is.EqualTo(AlbumOnePlayCount));
        Assert.That(albums[0].ImageUrl, Is.EqualTo(AlbumOneImageUrl));

        Assert.That(albums[1].Name, Is.EqualTo(AlbumTwoName));
        Assert.That(albums[1].PlayCount, Is.EqualTo(AlbumTwoPlayCount));
        Assert.That(albums[1].ImageUrl, Is.EqualTo(AlbumTwoImageUrl));
    }
    private void VerifyAlbumDetailsAreCorrect(AlbumDetailsBL album)
    {
        Assert.That(album.Name, Is.EqualTo(AlbumDetailsName));
        Assert.That(album.ArtistName, Is.EqualTo(AlbumDetailsArtistName));
        Assert.That(album.ImageUrl, Is.EqualTo(AlbumDetailsImageUrl));

        var tracks = album.Tracks.ToList();
        Assert.That(tracks[0].Name, Is.EqualTo(AlbumTrackOneName));
        Assert.That(tracks[0].DurationInSeconds, Is.EqualTo(AlbumTrackOneDurationInSeconds));

        Assert.That(tracks[1].Name, Is.EqualTo(AlbumTrackTwoName));
        Assert.That(tracks[1].DurationInSeconds, Is.EqualTo(AlbumTrackTwoDurationInSeconds));
    }
}