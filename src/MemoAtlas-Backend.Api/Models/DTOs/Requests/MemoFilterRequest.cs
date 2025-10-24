using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MemoAtlas_Backend.Api.Models.DTOs.Requests;

public class MemoFilterRequest
{
    [BindRequired]
    public DateOnly StartDate { get; set; }

    [BindRequired]
    public DateOnly EndDate { get; set; }
}