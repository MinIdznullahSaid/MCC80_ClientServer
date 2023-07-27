﻿using API.Utilities.Enums;

namespace API.DTOs.RoomDtos;

public class TodayBookedRoomDto
{
    //Booking
    public Guid BookingGuid { get; set; }

    //Room
    public string RoomName { get; set; }
    public StatusLevel Status { get; set; }
    public int Floor { get; set; }

    //Employee Name
    public string BookedBy { get; set; }
}
