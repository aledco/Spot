namespace Spot.Business.Models.Spotify
{
    public class SpotifySearchCriteria
    {
        public string Query { get; set; } = String.Empty;

        public IList<SpotifyType> Types { get; set; } = [];

        public string BuildQueryString()
        {
            var queryString = $"?q={this.Query}";
            if (this.Types.Any())
            {
                queryString += "&type=" + string.Join(",", this.Types);
            }

            return queryString;
        }
    }
}
