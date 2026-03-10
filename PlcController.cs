using System;
using Microsoft.AspNetCore.Mvc;
using S7.Net;

[ApiController]
[Route("api/[controller]")]
public class PlcController : ControllerBase
{
    private static Plc _plc = new Plc(CpuType.S71200, "192.168.1.10", 0, 1);

    [HttpGet("data")]
    public IActionResult GetPlcData()
    {
        try
        {
            if (!_plc.IsConnected) _plc.Open();

            // Đọc dữ liệu từ DB1 (Ví dụ)
            var data = new PlcDataDto
            {
                MotorStatus = (bool)_plc.Read("DB1.DBX0.0"),
                Speed = (short)_plc.Read("DB1.DBW2"),
                Temperature = ((uint)_plc.Read("DB1.DBD4")).ToFloat(),
                ProductionCount = (int)_plc.Read("DB1.DBD8"),
                MachineName = "SIEMENS_LINE_01"
            };

            return Ok(data);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}