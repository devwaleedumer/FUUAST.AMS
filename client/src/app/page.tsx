"use client";

import Header from "@/components/Layouts/Home/Header";
import Hero from "@/components/Home/Hero";
import Heading from "@/components/shared/Heading";
import ImageGallery from "@/components/shared/ImageGallery";
import Requirement from "@/components/Home/Requirement";

const images = [
  // "/UniPhotos/photo1.jpeg",
  "/UniPhotos/photo4.png",
  "/UniPhotos/photo2.jpg",
  "/UniPhotos/photo3.png",
  "/UniPhotos/photo5.jpeg",
  "/UniPhotos/photo6.jpeg",
  "/UniPhotos/photo7.jpeg",
  "/UniPhotos/photo4.png",
  "/UniPhotos/photo2.jpg",

]

export default function Home() {
  return (
    <>
    <Heading 
    title="Home"
     keywords="FUUAST, FUUAST ISB, FUUAST Islamabad, Admissions, Islamabad Admissions, Admissions, Apply"
    description="Apply for Federal Urdu University of Arts, Science & Technology Islamabad"
    />
    <Header/>
    <Hero/>
    <Requirement/>
    <ImageGallery 
     title={`Beautiful Campus of FUUAST  ❤️`} 
     description={`Explore our diverse range of academic programs and find the perfect fit for your educational journey.
`}
     images={images}
     />
    </>
  );
}
