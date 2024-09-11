import Image from "next/image";
import React, { FC } from "react";
import { AspectRatio } from "../ui/aspect-ratio";

type Props = {
  images: string[];
  title: string,
  description: string
};

const ImageGallery: FC<Props> = ({ images,title,description }) => {
  return (
    <section id="imageGallery" className="w-full py-12 md:py-24 lg:py-32">
        <div className=" container  px-4 md:px-6">
            <div className="space-y-4 text-center">
                <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                    {title}
                </h2>
                <p className="max-w-[700px] mx-auto text-muted-foreground md:text-xl">
                Explore our diverse range of academic programs and find the perfect fit for your educational journey.
              </p>
            </div>
              <div className="columns-3 md:gap-5 mt-8 mx-auto aspect-square gap-2">
      {images.map((image, index) => (
        <div key={index + "image"} className=" break-inside-avoid mb-8">
          <div className="relative overflow-hidden rounded-xl">
            <img
           loading="lazy"
            className="h-auto max-w-full rounded-xl transition-all hover:scale-125 duration-200 cursor-pointer"
            src={image}
            alt="Gallery image"
          />
          </div>
        </div>
      ))}

       
    </div>
        </div>
       </section>
  );
};

export default ImageGallery;
