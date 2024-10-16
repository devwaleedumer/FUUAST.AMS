import { AspectRatio } from "@/components/ui/aspect-ratio";
import Image from "next/image";
import React, { FC } from "react";
import { Button } from "../ui/button";

type Props = {};

const Hero: FC<Props> = ({}) => {
  return (
    // <section className="flex-1">
    //   <div className="w-full py-12 md:py-24 lg:py-32">
    //     <div className="container px-4 md:px-6">
    //       <div className="grid md:grid-cols-2 gap-8 items-center">
    //         {/* container one */}
    //         <div>
    //           <h1 className="text-3xl font-bold tracking-tight  md:text-5xl">
    //             The Federal Urdu University of Arts, Science & Technology
    //             Islamabad
    //           </h1>
    //           <p className="mt-4 text-muted-foreground md:text-lg">
    //             Becoming a leading university with quality education and
    //             research in a highly competitive global environment.To emerge as
    //             a leading department for excellence in engineering to produce
    //             highly motivated graduates. 
    //           </p>
    //         </div>
    //         {/* container two */}
    //         {/* <AspectRatio ratio={16 / 9}> */}
    //           <div className=" h-[450px] relative">
    //             <Image
    //             src="/UniPhotos/photo7.jpeg"
    //             loading="lazy"
    //             fill
    //             alt="University Campus"
    //             className="h-full w-full rounded-xl object-cover aspect-[600/400]"
    //           />
    //           </div>
    //         {/* </AspectRatio> */}
    //       </div>
    //     </div>
    //   </div>
    // </section>
    <section className="relative overflow-hidden bg-gradient-to-br from-green-50 to-emerald-100 py-12 sm:py-16 lg:py-20 xl:py-24 pt-[84.67px] ">
      <div className="container mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid gap-8 lg:grid-cols-2 lg:gap-16">
          <div className="flex flex-col justify-center space-y-6 text-center lg:text-left">
            <h1 className="text-4xl font-extrabold tracking-tight text-gray-900 sm:text-5xl md:text-6xl">
              Grow Your Future at{" "}
              <span className="text-green-600">FUUAST Islamabad</span>
            </h1>
            <p className="mx-auto max-w-3xl text-xl text-gray-700 lg:mx-0">
              Cultivate knowledge, nurture ambition, and branch out into new possibilities. 
              Join a thriving community where ideas flourish and futures take root.
            </p>
            <div className="flex flex-col space-y-4 sm:flex-row sm:space-x-4 sm:space-y-0 lg:justify-start">
              <Button size="lg" className="bg-green-600 text-white hover:bg-green-700 text-lg">
                Apply Now
              </Button>
              <Button 
                variant="outline" 
                size="lg" 
                className="text-green-600 border-green-600 hover:bg-green-50 text-lg"
              >
                Explore Programs
              </Button>
            </div>
          </div>
          <div className="relative hidden lg:block">
            <Image
              src="/UniPhotos/photo10.jpg"
              alt="Diverse students on campus"
              width={600}
              height={600}
              className="rounded-lg object-cover shadow-xl"
              priority
            />
            <div className="absolute -bottom-6 -left-6 h-48 w-48 rounded-full bg-green-200 opacity-70"></div>
            <div className="absolute -right-6 -top-6 h-48 w-48 rounded-full bg-emerald-200 opacity-70"></div>
          </div>
        </div>
      </div>
      <div className="absolute bottom-0 left-0 right-0">
        <svg
          viewBox="0 0 1440 120"
          fill="none"
          xmlns="http://www.w3.org/2000/svg"
          className="w-full"
        >
          <path
            d="M0 120L60 112C120 104 240 88 360 80C480 72 600 72 720 76C840 80 960 88 1080 92C1200 96 1320 96 1380 96L1440 96V120H1380C1320 120 1200 120 1080 120C960 120 840 120 720 120C600 120 480 120 360 120C240 120 120 120 60 120H0Z"
            fill="white"
          />
        </svg>
      </div>
    </section>
  );
};

export default Hero;
