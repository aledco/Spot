using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spot.Business.Models.Spotify
{
    public class SpotifyTrack
    {
        public object Album { get; set; }

        public SimplifiedSpotifyArtist[] Artists { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
