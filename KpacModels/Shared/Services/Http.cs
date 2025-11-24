// using System.Text;
// using RestSharp;
//
// namespace KpacModels.Shared.Services;
//
// public static class Http
// {
//
//     /// <summary>
//     /// Realizar una consulta Http - Post
//     /// </summary>
//     /// <param name="url">Url a donde queremos realizar la petición</param>
//     /// <param name="bearerToken">Token Bearer</param>
//     /// <param name="payload">Envio de payload</param>
//     /// <param name="bodyJsonString">Enviar un json string en el body</param>
//     /// <param name="fileXml">String del Xml para enviarlo como archivo</param>
//     /// <param name="headers">Diccionario con headers</param>
//     /// <param name="formData">Diccionario para enviar texto en el FromData</param>
//     /// <param name="formDataFile">Diccionario para enviar archivos en el FromData</param>
//     /// <returns>Retorna un objeto tipo <see cref="RestResponse"/> RestResponse</returns>
//     /// <exception cref="ArgumentException">Lanza Argument Exception si la Url es nula o vaciá</exception>
//     /// <exception cref="ApplicationException">Lanza Application Exception si no se pudo realizar la petición</exception>
//     public static async Task<RestResponse> PostAsync(
//     string url,
//     string? bearerToken = null,
//     object? payload = null,
//     string? bodyJsonString = null,
//     Dictionary<string, string>? headers = null,
//     Dictionary<string, string>? formData = null,
//     Dictionary<string, string>? formDataFile = null)
// {
//     if (string.IsNullOrEmpty(url))
//         throw new ArgumentException("URL cannot be null or empty.", nameof(url));
//
//     var options = new RestClientOptions(url)
//     {
//         RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
//     };
//
//     using var client = new RestClient(options);
//     var request = CreateRequest(bearerToken, payload, bodyJsonString, headers, formData, formDataFile);
//
//     // Logs.Info(JsonConvert.SerializeObject(request));
//     try
//     {
//         var response = await client.ExecutePostAsync(request);
//         return response;
//     }
//     catch (Exception ex)
//     {
//         throw new ApplicationException("An error occurred while making the POST request.", ex);
//     }
// }
//
// private static RestRequest CreateRequest(string? bearerToken,
//     object? payload,
//     string? bodyJsonString,
//     Dictionary<string, string>? headers,
//     Dictionary<string, string>? formData,
//     Dictionary<string, string>? formDataFile)
// {
//     var request = new RestRequest();
//
//     AddAuthorizationHeader(request, bearerToken);
//     AddHeaders(request, headers);
//     AddPayload(request, payload);
//     AddBodyJsonString(request, bodyJsonString);
//     AddFormData(request, formData);
//     AddFormDataFiles(request, formDataFile);
//
//     return request;
// }
//
// private static void AddBodyJsonString(RestRequest request, string? bodyJsonString)
// {
//     if (bodyJsonString != null)
//         request.AddStringBody(bodyJsonString, DataFormat.Json);
// }
//
// private static void AddAuthorizationHeader(RestRequest request, string? bearerToken)
// {
//     if (!string.IsNullOrEmpty(bearerToken))
//         request.AddHeader("Authorization", $"Bearer {bearerToken}");
// }
//
// private static void AddHeaders(RestRequest request, Dictionary<string, string>? headers)
// {
//     if (headers != null)
//     {
//         foreach (var header in headers)
//             request.AddHeader(header.Key, header.Value);
//     }
// }
//
// private static void AddPayload(RestRequest request, object? payload)
// {
//     if (payload != null)
//         request.AddBody(payload);
// }
//
// private static void AddFormData(RestRequest request, Dictionary<string, string>? formData)
// {
//     if (formData != null)
//     {
//         foreach (var data in formData)
//             request.AddParameter(data.Key, data.Value);
//     }
// }
//
// private static void AddFormDataFiles(RestRequest request, Dictionary<string, string>? formDataFile)
// {
//     if (formDataFile != null)
//     {
//         foreach (var file in formDataFile)
//         {
//             var fileBytes = Encoding.UTF8.GetBytes(file.Value);
//             request.AddFile(file.Key, fileBytes, file.Key, "application/octet-stream");
//         }
//     }
// }
//     
//     public static async Task<RestResponse> GetAsync(string url, string? bearerToken = null,Dictionary<string, string>? headers = null)
//     {
//         var client = new RestClient(url);
//         var request = new RestRequest();
//
//         // Agregar token de autorización
//         if (!string.IsNullOrEmpty(bearerToken))
//             request.AddHeader("Authorization", $"Bearer {bearerToken}");
//
//         AddHeaders(request, headers);
//         // Ejecutar la petición y obtener el contenido
//         var response = await client.ExecuteGetAsync(request);
//         return response;
//     }
// }