import PagingData from '../../shared/models/paging-data';
import { Artist } from './artist';

export default interface ArtistsPaging {
  artists: Artist[];
  pagingData: PagingData;
}
