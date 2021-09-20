using AutoMapper;
using ChallengeSBI.WebApi.Helpers;
using ChallengeSBI.WebApi.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace ChallengeSBI.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostController(ILogger<PostController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ServerPost, Salida>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                    .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.title));
            });
            _mapper = config.CreateMapper();
        }

        [HttpGet("GetPostsById/{id}")]
        public ActionResult GetPostsById(int id)
        {
            try
            {
                var response = _mediator.Send(new Api());
                var listPost = JsonConvert.DeserializeObject<List<ServerPost>>(response.Result);
                var source = listPost.Where(x => x.id == id).FirstOrDefault();
                var mappedOutput = _mapper.Map<ServerPost, Salida>(source);
                return Json(mappedOutput);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpGet("GetAllPost")]
        public ActionResult GetAllPost()
        {
            try
            {
                var response = _mediator.Send(new Api());
                var listPost = JsonConvert.DeserializeObject<List<ServerPost>>(response.Result);
                return Json(listPost);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
