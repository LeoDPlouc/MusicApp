using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    partial class Artist
    {
        public async static Task<List<Artist>> ListAllArtists()
        {
            return await MusicDataBase.ListArtist();
        }
        public async static Task<Artist> SelectArtistById(int id)
        {
            var artists = await ListAllArtists();
            return artists.Find((Artist a) =>
            {
                return a.Id == id;
            });
        }
        public async static Task<Artist> SelectArtistByName(string name)
        {
            var artists = await ListAllArtists();
            return artists.Find((Artist a) =>
            {
                return a.Name == name;
            });
        }
        public async static Task<List<Artist>> SearchArtistByName(string query)
        {
            var artistsTask = ListAllArtists();

            Regex re = new Regex(query);

            var artists = await artistsTask;

            return artists.FindAll((Artist a) =>
            {
                return re.IsMatch(a.Name);
            });

        }

        public async Task<List<Album>> SelectAlbumFromArtist()
        {
            var albums = await Album.SelectAllAlbum();
            return albums.FindAll((Album a) =>
            {
                return a.Artist.Id == Id;
            });
        }
    }
}
