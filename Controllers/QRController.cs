using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
namespace AsistenciasApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class QRController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GenerateQrCode(string link)
        {
            try
            {
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(link, QRCodeGenerator.ECCLevel.Q);
                BitmapByteQRCode bitmapByteQRCode = new BitmapByteQRCode(qrCodeData);
                var bitMap = bitmapByteQRCode.GetGraphic(20);

                using var ms = new MemoryStream();
                await ms.WriteAsync(bitMap);
                byte[] data = ms.ToArray();
                return Ok(Convert.ToBase64String(data));

            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
