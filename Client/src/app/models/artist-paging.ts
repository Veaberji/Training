import { Artist } from './artist';
import PagingData from './paging-data';

export default interface ArtistsPaging {
  artists: Artist[];
  pagingData: PagingData;
}
