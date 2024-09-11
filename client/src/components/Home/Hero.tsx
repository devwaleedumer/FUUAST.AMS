import { AspectRatio } from "@/components/ui/aspect-ratio";
import Image from "next/image";
import React, { FC } from "react";

type Props = {};

const Hero: FC<Props> = ({}) => {
  return (
    <section className="flex-1">
      <div className="w-full py-12 md:py-24 lg:py-32">
        <div className="container px-4 md:px-6">
          <div className="grid md:grid-cols-2 gap-8 items-center">
            {/* container one */}
            <div>
              <h1 className="text-3xl font-bold tracking-tight  md:text-5xl">
                The Federal Urdu University of Arts, Science & Technology
                Islamabad
              </h1>
              <p className="mt-4 text-muted-foreground md:text-lg">
                Becoming a leading university with quality education and
                research in a highly competitive global environment.To emerge as
                a leading department for excellence in engineering to produce
                highly motivated graduates. 
              </p>
            </div>
            {/* container two */}
            {/* <AspectRatio ratio={16 / 9}> */}
              <div className=" h-[450px] relative">
                <Image
                src="/UniPhotos/photo7.jpeg"
                loading="lazy"
                fill
                alt="University Campus"
                className="h-full w-full rounded-xl object-cover aspect-[600/400]"
              />
              </div>
            {/* </AspectRatio> */}
          </div>
        </div>
      </div>
    </section>
  );
};

export default Hero;
