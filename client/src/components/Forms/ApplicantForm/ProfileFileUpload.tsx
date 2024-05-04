/* eslint-disable @next/next/no-img-element */
import React, { useEffect, useState } from "react";
import { AspectRatio } from "../../ui/aspect-ratio";
import Image from "next/image";
import { FaTrashAlt } from "react-icons/fa";
import { FileUpload } from "../../shared/FileUpload";

interface ProfileFileUploadProps extends React.InputHTMLAttributes<HTMLInputElement> {
  value: any;
  onChange: (value: any) => void;
  isValid: boolean
}

const ProfileFileUpload = React.forwardRef<HTMLInputElement, ProfileFileUploadProps>(
  ({ isValid, value, onChange, ...props }, ref) => {
    {
      const [preview, setPreview] = useState<string | undefined>();
      return (
        <>
          {preview && (
            <div className="w-full flex justify-center items-center">
              <div
                className={`border relative w-[100px] h-[100px] rounded-full overflow-hidden ring-2 ring-primary hover:opacity-70 transition-opacity duration-100 ease-in-out`}
              >
                <AspectRatio ratio={1 / 1} className="relative">
                  <Image src={preview} alt="upload item" fill />
                </AspectRatio>
                <FaTrashAlt
                  className="h-4 w-5 z-30 absolute text-green-700 hover:text-green-500 top-[17%] left-[68%] cursor-pointer"
                  onClick={() => {
                    setPreview(undefined);
                    onChange(null);
                  }}
                />
              </div>
            </div>
          )}
          <FileUpload onChange={onChange} value={value} sizeDescription="Upload Image of size 4MB" isValid={isValid} preview={preview} setPreview={setPreview} />
        </>
      );
    }
  }
);
ProfileFileUpload.displayName = "ProfileFileUpload";
export { ProfileFileUpload };
