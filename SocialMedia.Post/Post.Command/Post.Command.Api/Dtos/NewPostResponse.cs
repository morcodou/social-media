using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Post.Common.Dtos;

namespace Post.Command.Api.Dtos
{
    public class NewPostResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}