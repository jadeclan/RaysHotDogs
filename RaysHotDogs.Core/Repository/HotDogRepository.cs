using Newtonsoft.Json;
using RaysHotDogs.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.OS;

namespace RaysHotDogs.Core.Repository
{
    public class HotDogRepository
    {
        private static List<HotDogGroup> hotDogGroups = new List<HotDogGroup>();
        private string url = "http://gillcleerenpluralsight.blob.core.windows.net/files/hotdogs.json";

        // We load the data with a call created in the constructor.
        public HotDogRepository()
        {
            Task.Run(() => this.LoadDataAsync(url)).Wait();
        }
        private async Task LoadDataAsync(string uri)
        {
            if (hotDogGroups != null)
            {
                string responseJsonString = null;
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        // asks http client to get data aynchronously from the uri and it
                        // returns a Task
                        Task<HttpResponseMessage> getResponse = httpClient.GetAsync(uri);
                        // We await the response
                        HttpResponseMessage response = await getResponse;
                        // Get data from the response and read out the data asynchronously
                        // as a string
                        responseJsonString = await response.Content.ReadAsStringAsync();
                        // Next we parse it using JsonConvert.
                        hotDogGroups = JsonConvert.DeserializeObject<List<HotDogGroup>>(responseJsonString);
                        // We now have our hotDogGroups coming from a service.
                    }
                    catch (Exception ex)
                    {
                        Android.Util.Log.WriteLine(0,"Error getting data: ", "" + ex);
                    }
                }
            }
        }
        // Note the following hot dog list is being commented out in favor of 
        // using a REST servicen that is called from the method above.
        //
        //private static List<HotDogGroup> hotDogGroups = new List<HotDogGroup>()
        //{
        //    new HotDogGroup()
        //    {
        //        HotDogGroupId =1, Title="meat lovers",
        //        ImagePath = "", HotDogs = new List<HotDog>()
        //        {
        //            new HotDog()
        //            {
        //                HotDogId = 1,
        //                Name = "Regular hot dog",
        //                ShortDescription = "the best there is",
        //                Description = "somehting1",
        //                ImagePath = "hotdog1",
        //                Available = true,
        //                PrepTime = 10,
        //                Ingredients = new List<string>()
        //                {
        //                    "Regular bun",
        //                    "other stuff"
        //                },
        //                Price = 8,
        //                IsFavorite = true
        //            },
        //            new HotDog()
        //            {
        //                HotDogId = 2,
        //                Name = "Haute Dog",
        //                ShortDescription = "blah blah",
        //                Description = "whatever 2",
        //                ImagePath = "hotdog2",
        //                Available = true,
        //                PrepTime = 15,
        //                Ingredients = new List<string>()
        //                {
        //                    "Baked bun",
        //                    "Gourmet Sausage"
        //                },
        //                Price = 10,
        //                IsFavorite = false
        //            },
        //            new HotDog()
        //            {
        //                HotDogId = 3,
        //                Name = "Extra Long",
        //                ShortDescription = "For when a regular one isn't enough",
        //                Description = "yaddda yadda",
        //                ImagePath = "hotdog3",
        //                Available = true,
        //                PrepTime = 8,
        //                Ingredients = new List<string>()
        //                {
        //                    "Extra long bun",
        //                    "other stuff X 2"
        //                },
        //                Price = 8,
        //                IsFavorite = true
        //            }
        //        }
        //    },
        //    new HotDogGroup()
        //    {
        //        HotDogGroupId = 2,Title = "Veggie Lover", ImagePath="",
        //        HotDogs = new List<HotDog>()
        //        {
        //                                new HotDog()
        //            {
        //                HotDogId = 4,
        //                Name = "hot dog a la mode",
        //                ShortDescription = "hmmm",
        //                Description = "somehting  IV",
        //                ImagePath = "hotdog4",
        //                Available = true,
        //                PrepTime = 11,
        //                Ingredients = new List<string>()
        //                {
        //                    "Regular bun",
        //                    "Ice Cream"
        //                },
        //                Price = 12,
        //                IsFavorite = true
        //            },
        //            new HotDog()
        //            {
        //                HotDogId = 5,
        //                Name = "chocolate Dog",
        //                ShortDescription = "blah blah X 2",
        //                Description = "whatever 5",
        //                ImagePath = "hotdog5",
        //                Available = true,
        //                PrepTime = 17,
        //                Ingredients = new List<string>()
        //                {
        //                    "Baked bun",
        //                    "Gourmet Sausage",
        //                    "Chocolate Ice Cream"
        //                },
        //                Price = 20,
        //                IsFavorite = false
        //            },
        //            new HotDog()
        //            {
        //                HotDogId = 6,
        //                Name = "Extra Long something",
        //                ShortDescription = "hmemme 6",
        //                Description = "yaddda yadda X 6",
        //                ImagePath = "hotdog6",
        //                Available = true,
        //                PrepTime = 1,
        //                Ingredients = new List<string>()
        //                {
        //                    "Extra long bun",
        //                    "just mustard"
        //                },
        //                Price = 18,
        //                IsFavorite = false
        //            }
        //        }
        //    }
        //};

        public List<HotDog> GetAllHotDogs()
        {
            IEnumerable<HotDog> hotDogs =
                from hotDogGroup in hotDogGroups
                from hotDog in hotDogGroup.HotDogs

                select hotDog;
            return hotDogs.ToList<HotDog>();
        }
        public HotDog GetHotDogById(int hotDogId)
        {
            IEnumerable<HotDog> hotDogs =
                from hotDogGroup in hotDogGroups
                from hotDog in hotDogGroup.HotDogs
                where hotDog.HotDogId == hotDogId
                select hotDog;
            return hotDogs.FirstOrDefault();
        }
        public List<HotDogGroup> GetGroupedHotDogs()
        {
            return hotDogGroups;
        }
        public List<HotDog> GetHotDogsForGroup(int hotDogGroupId)
        {
            var group = hotDogGroups.Where(h => h.HotDogGroupId == hotDogGroupId).FirstOrDefault();

            if (group != null)
            {
                return group.HotDogs;
            }
            return null;
        }
        public List<HotDog> GetFavoriteHotDogs()
        {
            IEnumerable<HotDog> hotDogs =
                from HotDogGroup in hotDogGroups
                from HotDog in HotDogGroup.HotDogs
                where HotDog.IsFavorite
                select HotDog;

            return hotDogs.ToList<HotDog>();
        }


    }
}
