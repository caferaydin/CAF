using CAF.Application.Models.Common;

namespace CAF.Application.Helpers
{
    public static class ResultHelpers
    {

        public static int GetResultCode<T>(T data)
        {
            if (data == null)
                return -1;

            // Eğer data bir string ise, "success" kontrolü
            if (data is string strData)
                return strData.Equals("success", StringComparison.OrdinalIgnoreCase) ? 0 : -1;

            // Eğer data bir bool ise
            if (data is bool boolData)
                return boolData ? 0 : -1;

            return 0; // Diğer durumlar için başarılı varsayıyoruz
        }

    }


}
