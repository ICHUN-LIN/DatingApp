using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Core;
using CloudinaryDotNet.Actions;
using DatingApp.api.Data;
using DatingApp.api.DTOS;
using DatingApp.api.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DatingApp.api.Models;
using System.Linq;

namespace DatingApp.api.Controllers
{
    // [ApiController] sometimes stop exception early, so we can't get our debug break point 
    // comment it can help debugging
    //[Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IOptions<CloudinarySettings> _couldinaryconfig;
        private readonly IDatingRepository _repository;
        private readonly IMapper _mapper;
        private readonly Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repository,
                               IMapper mapper,
                               IOptions<CloudinarySettings> couldinaryconfig)
        {
            this._repository = repository;
            this._mapper = mapper;
            this._couldinaryconfig = couldinaryconfig;

            Account acc = new Account(
                _couldinaryconfig.Value.CloudName,
                _couldinaryconfig.Value.ApiKey,
                _couldinaryconfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }


        [HttpGet("{id}",Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photofromRep = await _repository.GetPhotos(id);
            var returphoto = _mapper.Map<PhotoForReturnDto>(photofromRep);
            return Ok(returphoto);

        }

/*from form -> this is important, sometimes it goes wrong*/
        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotoForCreationDTO photoDto){
            
            //if now user id != modify user id ==> no right to change
            if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRep = await _repository.GetUser(userId);

            var file = photoDto.File;
            var uploadResult = new ImageUploadResult();

            if( file.Length>0 )
            {
                using(var stream= file.OpenReadStream()){

                    var imageUploadparam = new ImageUploadParams(){
                        File = new FileDescription(file.Name,stream),
                        //if photo is too large, focuse on face and square shape and crop.
                        Transformation = new Transformation().Width("500").Height("500").Crop("fill").Gravity("face")
                    };

                     uploadResult = this._cloudinary.Upload(imageUploadparam);
                }
               
            }

            photoDto.Url = uploadResult.Uri.ToString();
            photoDto.PublicId = uploadResult.PublicId;

            var addphoto = _mapper.Map<Photo>(photoDto);

            if(!userFromRep.Photos.Any(x=>x.IsMain == true))
                addphoto.IsMain = true;

            userFromRep.Photos.Add(addphoto);
            if(await _repository.SaveAll())
            {
                var photoForReturn = _mapper.Map<PhotoForReturnDto>(addphoto);
                return CreatedAtRoute("GetPhoto",new { userId = userId, id = addphoto.Id}, photoForReturn);
            }

            return BadRequest("Could Not add Photo");

        }

        [HttpPost("{id}/SetMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            //if now user id != modify user id ==> no right to change
            if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var user = await _repository.GetUser(userId);

            if(!user.Photos.Any(p=> p.Id == id))
                return Unauthorized();


            
            /*
            either is fine:
            
            var photo = await _repository.GetPhotos(id);

            if(photo.IsMain)
                return BadRequest("already main photo");

            
            var currentMainPhoto = await _repository.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            if(await _repository.SaveAll())
                return NoContent();

            return BadRequest("could not save photo to main");
            */

            var photo = user.Photos.FirstOrDefault(a => a.Id == id);
            var currentMainPhoto = user.Photos.FirstOrDefault( a => a.IsMain == true);

            currentMainPhoto.IsMain = false;
            photo.IsMain = true;

            if(await _repository.SaveAll())
                return NoContent();

            return BadRequest("could not save photo to main");

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletPhoto(int userId, int id)
        {
            //if now user id != modify user id ==> no right to change
            if(userId!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var user = await _repository.GetUser(userId);

            if(!user.Photos.Any(p=> p.Id == id))
                return Unauthorized();
            
            var photofromRep = await _repository.GetPhotos(id);

            if(photofromRep.IsMain)
                return BadRequest("can't delete main photo");
                
            if(photofromRep.PublicId != null )
            {
                var deleteParam = new DeletionParams(photofromRep.PublicId);

                var result = _cloudinary.Destroy(deleteParam);

                if( result.Result == "ok"){
                    _repository.Delet(photofromRep);
                }

                if(await _repository.SaveAll())
                    return Ok();
            }

            return BadRequest("Fail to delete the photo");
            
        }

    }
}