using System;
using System.Collections.Generic;
using NRestful;

namespace NRestfulConsole {
    public class Program {
        public static void Main(string[] args) {

            Console.WriteLine("Rest Easy Testing");

            RequestTest();

            Console.ReadLine();

        }

        private static async void RequestTest() {

            /***************
             * WebApi URL:
             ***************/
            const string Url = "http://localhost:3034/api/";

            var client = new Client(Url);

            //Send a GET request:
            var response = client.RequestAsync<string>(new Request {
                EndPoint = new EndPoint {
                    Method = Method.GET,
                    Uri = "sample/{id}"
                },
                UrlSegment = { { "id", "5" } }
            });

            var result = await response;

            Console.WriteLine(result.Content);

            Console.ReadLine();

        }


    }

    public class Login {
        public string Email { set; get; }
        public string Password { set; get; }
        public bool Persist { set; get; }
    }
    public class ServiceEvent {
        public string ID { get; set; }
        public string Title { set; get; }
        public List<string> Tags { set; get; }
        public string Description { set; get; }
    }

}
