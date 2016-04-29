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

            //Send a POST request:
            var response = client.RequestAsync<User>(new Request {
                EndPoint = new EndPoint {
                    Method = Method.GET,
                    Uri = "sample/account/1"
                }
            });

            var result = await response;

            Console.WriteLine("{0} {1} ({2})", result.Content.FirstName,
                                            result.Content.LastName,
                                            result.Content.Email);

            Console.ReadLine();

        }


    }

    public class User {
        public string Email { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
    }
    public class ServiceEvent {
        public string ID { get; set; }
        public string Title { set; get; }
        public List<string> Tags { set; get; }
        public string Description { set; get; }
    }

}
