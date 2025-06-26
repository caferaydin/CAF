using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace CAF.Application.Helpers;

public static class ApplicationHelpers
{
    private static Random random = new Random();
    public static string GenerateCode(string prefix)
    {
        string segment1 = GenerateRandomSegment(3);
        string segment2 = GenerateRandomSegment(2);
        string segment3 = GenerateRandomSegment(3);

        return $"{prefix}-{segment1}-{segment2}-{segment3}";
    }

    private static string GenerateRandomSegment(int length)
    {
        const string chars = "0123456789";
        StringBuilder result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }

    //public static async Task<DocumentModel> DocumentUpload(string documentFolder, IFormFile uploadFile)
    //{
    //    string fileName = string.Empty;
    //    string filePath = string.Empty;

    //    if (uploadFile != null && uploadFile.Length > 0)
    //    {
    //        // Yükleme dizinini belirleyin (örneğin, wwwroot/contractdocs)
    //        string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documentFolder);

    //        // Dosya adını belirleyin
    //        fileName = $"{Guid.NewGuid()}_{uploadFile.FileName}";

    //        // Tam dosya yolunu oluşturun
    //        filePath = Path.Combine(uploadPath, fileName);

    //        // Yükleme dizinini oluşturun
    //        if (!Directory.Exists(uploadPath))
    //        {
    //            Directory.CreateDirectory(uploadPath);
    //        }

    //        // Dosyayı yükleyin
    //        using (var fileStream = new FileStream(filePath, FileMode.Create))
    //        {
    //            await uploadFile.CopyToAsync(fileStream);
    //        }

    //        return new DocumentModel
    //        {
    //            FileName = fileName,
    //            FilePath = filePath,
    //            wwwRoot = uploadPath
    //        };
    //    }

    //    return null;
    //}

    public static double ConvertMinutesToHours(double minutes)
    {
        return minutes / 60.0;
    }


    public static string CleanPhoneNumber(string phone)
    {
        // Telefon numarasını yalnızca rakamlarla bırakacak şekilde temizler
        return Regex.Replace(phone, @"[^\d]", "");
    }

    public static Dictionary<TKey, string[]> GetErrors<TKey>(IEnumerable<KeyValuePair<TKey, IEnumerable<string>>> errors)
    {
        return errors
            .Where(kvp => kvp.Value.Any())
            .ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.ToArray()
            );
    }

    public static async Task HandleSaveErrorAsync<TModel>(TModel model, ModelStateDictionary modelState, ITempDataDictionary tempData)
    {
        tempData["Errors"] = JsonConvert.SerializeObject(GetErrors(
            modelState.Where(ms => ms.Value.Errors.Count > 0)
                      .ToDictionary(
                          kvp => kvp.Key,
                          kvp => kvp.Value.Errors.Select(e => e.ErrorMessage)
                      )));
        tempData["Message"] = "Kayıt işlemi başarısız oldu.";
        tempData["MessageType"] = "error";
    }



}
