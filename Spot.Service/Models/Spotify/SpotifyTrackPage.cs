using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spot.Business.Models.Spotify
{
    public class SpotifyTrackPage
    {
        public int Limit { get; set; }
        public int Total { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public SpotifySavedTrack[] Items { get; set; }
    }
}
