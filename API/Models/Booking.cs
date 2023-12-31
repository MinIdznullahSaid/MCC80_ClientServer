﻿using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_booking")]
public class Booking : BaseEntity
{
    [Column("start_date")]
    public DateTime StartDate { get; set; }
    [Column("end_date")]
    public DateTime EndDate { get; set; }
    [Column("status")]
    public StatusLevel Status { get; set; }
    [Column("remarks")]
    public string Remark { get; set; }
    [Column("room_guid")]
    public Guid RoomGuid { get; set; }
    [Column("employee_guid")]
    public Guid EmployeeGuid { get; set; }

    //Cardinality
    public Room? Room { get; set; }
    public Employee? Employee { get; set; }
}

