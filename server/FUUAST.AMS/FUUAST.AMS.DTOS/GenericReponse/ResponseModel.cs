using FUUAST.AMS.SHARED.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUUAST.AMS.MODELS.GenericReponse
{
    public class ResponseModel
    {
        public object? Data { get; set; }
        public int? ResponseCode { get; set; }
        public string? ResponseMessage { get; set; }
        public bool? Success { get; set; }
        public List<ErrorModel>? Errors { get; set; }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ResponseModel()
        {
            Data = null;
            ResponseCode = 00;
            Success = false;
            Errors = new List<ErrorModel>();
        }

        public ResponseModel(bool isSuccess)
        {
            if (isSuccess)
                SuccessResponseModel();
        }

        public void SuccessResponseModel()
        {
            Data = null;
            ResponseCode = StatusCodes.Success;
            ResponseMessage = string.Empty;
            Success = true;
            Errors = new List<ErrorModel>();
        }

        public void ExceptionResponseModel()
        {
            Data = null;
            ResponseCode = StatusCodes.InternalServerError;
            ResponseMessage = string.Empty;
            Success = false;
            Errors = new List<ErrorModel>();
        }

        public void AddErrors(string? error)
        {
            if (error != null)
                Errors?.Add(new ErrorModel(error));
        }
    }

    public class ErrorModel
    {
        public string? ErrorMessage { get; set; }

        public ErrorModel(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}
