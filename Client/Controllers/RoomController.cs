using API.DTOs.EmployeeDtos;
using API.DTOs.RoomDtos;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Authorize(Roles = "Manager, Admin")]
public class RoomController : Controller
{
    private readonly IRoomRepository repository;

    public RoomController(IRoomRepository repository)
    {
        this.repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await repository.Get();
        var ListRoom = new List<Room>();

        if (result.Data != null)
        {
            ListRoom = result.Data.ToList();
        }
        return View(ListRoom);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Room newRoom)
    {

        var result = await repository.Post(newRoom);
        if (result.Status == "200")
        {
            TempData["Success"] = "Data berhasil masuk";
            return RedirectToAction(nameof(Index));
        }
        else if (result.Status == "409")
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        return RedirectToAction(nameof(Index));

    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await repository.Get(id);
        var ListRoom = new NewRoomDto();

        if (result.Data != null)
        {
            ListRoom = (NewRoomDto)result.Data;
        }
        return View(ListRoom);
    }

    [HttpPost]
    public async Task<IActionResult> Update(RoomDto updateRoom)
    {
        var result = await repository.Put(updateRoom.Guid, updateRoom);

        if (result.Code == 200)
        {
            TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
            return RedirectToAction("Index", "Room");
        }
        return RedirectToAction(nameof(Edit));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var result = await repository.Delete(guid);
        if (result.Status == "200")
        {
            TempData["Success"] = "Data Berhasil Dihapus";
        }
        else
        {
            TempData["Error"] = "Gagal Menghapus Data";
        }
        return RedirectToAction(nameof(Index));
    }



}
