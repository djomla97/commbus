using SharedResources.Interfaces;

namespace SharedResources
{
    public class Request : IRequest
    {
        public string Verb { get; set; }

        public string Noun { get; set; }

        public string Query { get; set; }

        public string Fields { get; set; }

        public Request()
        {

        }

        public Request(string verb, string noun, string query, string fields)
        {
            this.Verb = verb;
            this.Noun = noun;
            this.Query = query;
            this.Fields = fields;
        }
    }
}
