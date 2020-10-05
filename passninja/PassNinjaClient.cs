using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace passninja
{
    public class PassNinjaClient
    {
        public static string BASE_PATH = "https://api.passninja.com/v1";

        private static RestClient _client;
        public static string AccountId { get; set; }
        public PassData pass { get; set; }

        public PassNinjaClient(string accountId, string apiKey)
        {
            if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(apiKey))
            {
                throw new PassNinjaInvalidArgumentsException("Must provide both accountId and apiKey to PassNinjaClient constructor. PassNinjaClient(accountId: string, apiKey: string)");
            }

            _client = new RestClient(BASE_PATH);
            _client.AddDefaultHeader("x-account-id", accountId);
            _client.AddDefaultHeader("Content-Type", "application/json");
            _client.AddDefaultHeader("x-api-key", apiKey);
        }

        /// <summary>
        /// Create new NFC pass.
        /// </summary>
        /// <param name="passType"></param>
        /// <param name="passData"></param>
        /// <returns>  Result of creating a new NFC pass.</returns>
        public PassResponseData CreatePass(string passType, PassData passData)
        {
            PassResponseData passResponseData = new PassResponseData();
            try
            {
                if (string.IsNullOrWhiteSpace(passType))
                {
                    throw new PassNinjaInvalidArgumentsException("Must provide passType to PassNinjaClient.CreatePass method. PassNinjaClient.CreatePass(passType: string, passData: PassData)");
                }

                List<string> invalidKeys = ExtractInvalidKeys(passData);

                if (invalidKeys.Count > 0)
                {
                    throw new PassNinjaInvalidArgumentsException("Invalid templateStrings provided in clientPassData object. Invalid keys: " + string.Join(",", invalidKeys.ToArray()));
                }

                var postData = new PassRequestData();
                postData.passType = passType;
                postData.pass = passData;

                var request = new RestRequest("/passes", Method.POST);
                request.AddJsonBody(postData);
                IRestResponse response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    passResponseData = JsonConvert.DeserializeObject<PassResponseData>(response.Content);
                    if (passResponseData != null)
                        passResponseData.url = passResponseData.urls?.landing;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passResponseData;
        }

        /// <summary>
        /// Get data for an NFC pass.
        /// </summary>
        /// <param name="passType"></param>
        /// <param name="serialNumber"></param>
        /// <returns>PassResponseData</returns>
        public PassResponseData GetPass(string passType, string serialNumber)
        {
            PassResponseData passResponseData = new PassResponseData();
            try
            {
                if (string.IsNullOrWhiteSpace(passType) || string.IsNullOrWhiteSpace(serialNumber))
                {
                    throw new PassNinjaInvalidArgumentsException("Must provide both passType and serialNumber to PassNinjaClient.GetPass method. PassNinjaClient.GetPass(passType: string, serialNumber: string)");
                }

                var request = new RestRequest("/passes/" + passType + "/" + serialNumber, Method.GET);
                IRestResponse response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    passResponseData = JsonConvert.DeserializeObject<PassResponseData>(response.Content);
                    if (passResponseData != null)
                        passResponseData.url = passResponseData.urls?.landing;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passResponseData;
        }

        /// <summary>
        /// Update NFC pass.
        /// </summary>
        /// <param name="passType"></param>
        /// <param name="passData"></param>
        /// <returns>  Result of updating a NFC pass..</returns>
        public PassResponseData PutPass(string passType, string serialNumber, PassData passData)
        {
            PassResponseData passResponseData = new PassResponseData();
            try
            {
                if (string.IsNullOrWhiteSpace(passType) || string.IsNullOrWhiteSpace(serialNumber))
                {
                    throw new PassNinjaInvalidArgumentsException("Must provide both passType and serialNumber to PassNinjaClient.PutPass method. PassNinjaClient.PutPass(passType: string, serialNumber: string, passData: PassData)");
                }

                List<string> invalidKeys = ExtractInvalidKeys(passData);

                if (invalidKeys.Count > 0)
                {
                    throw new PassNinjaInvalidArgumentsException("Invalid templateStrings provided in clientPassData object. Invalid keys: " + string.Join(",", invalidKeys.ToArray()));
                }

                var postData = new PassRequestData();
                postData.passType = passType;
                postData.pass = passData;

                var request = new RestRequest("/passes/" + passType + "/" + serialNumber, Method.POST);
                request.AddJsonBody(postData);
                IRestResponse response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    passResponseData = JsonConvert.DeserializeObject<PassResponseData>(response.Content);
                    if (passResponseData != null)
                        passResponseData.url = passResponseData.urls?.landing;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return passResponseData;
        }

        /// <summary>
        ///  Set a currently existing pass to be invalid/inactive. Returns serial_number.
        /// </summary>
        /// <param name="passType"></param>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public string DeletePass(string passType, string serialNumber)
        { 
            try
            {
                if (string.IsNullOrWhiteSpace(passType) || string.IsNullOrWhiteSpace(serialNumber))
                {
                    throw new PassNinjaInvalidArgumentsException("Must provide both passType and serialNumber to PassNinjaClient.DeletePass method. PassNinjaClient.DeletePass(passType: string, serialNumber: string)");
                }
                var request = new RestRequest("/passes/" + passType + "/" + serialNumber, Method.DELETE);
                IRestResponse response = _client.Execute(request); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return serialNumber;
        }

        #region Private Methods

        /// <summary>
        /// Extracting Invalid keys
        /// </summary>
        /// <param name="passData"></param>
        /// <returns></returns>
        private List<string> ExtractInvalidKeys(PassData passData)
        {
            List<string> keysList = new List<string>();

            if (string.IsNullOrWhiteSpace(passData.discount))
            {
                keysList.Add("discount");
            }
            if (string.IsNullOrWhiteSpace(passData.memberName))
            {
                keysList.Add("memberName");
            }

            return keysList;
        }

        /// <summary>
        /// Fatch required keys for passtype
        /// </summary>
        /// <param name="passType"></param>
        /// <returns></returns>
        private List<string> FetchRequiredKeysSet(string passType)
        {
            List<string> keysList = new List<string>();
            try
            {
                var request = new RestRequest("/passes/keys/" + passType, Method.GET);

                IRestResponse response = _client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    keysList = JsonConvert.DeserializeObject<List<string>>(response.Content);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return keysList;
        }

        #endregion
    }
}
