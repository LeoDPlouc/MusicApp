using MusicApp.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicApp.Beans
{
    partial class Picture
    {
        public async static Task<List<Picture>> SelectAllPictures()
        {
            return await MusicDataBase.ListPicture();
        }
        public async static Task<Picture> SelectPictureByData(byte[] data)
        {
            var pictures = await SelectAllPictures();
            return pictures.Find((Picture p) =>
            {
                return p.Data == data;
            });
        }
        public async static Task<Picture> SelectPictureById(int id)
        {
            var pictures = await SelectAllPictures();
            return pictures.Find((Picture p) =>
            {
                return p.Id == id;
            });
        }

        public void Delete()
        {
            MusicDataBase.DeletePicture(Id);
        }
    }
}
