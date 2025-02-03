import { Button } from '@/components/ui/button'
import useScrollPosition from '@/hooks/useScrollYposition'
import { cn } from '@/lib/utils'
import { FUUASTIconOne } from '@/utils/fonts/font'
import { AuthRoutes } from '@/utils/routes/Routes'
import { Cross, Menu, X } from 'lucide-react'
import Image from 'next/image'
import Link from 'next/link'
import React, { FC, useState } from 'react'

type Props = {}

const Header: FC<Props> = () => {
  // latest scroll position
  const [showMobileNav, setShowMobileNav] = useState<boolean>(false)
  return (
    <header className={cn(`fixed z-10 w-full p-4  border-b bg-emerald-50 `)}>
        <div className="container flex items-center justify-between">
            <Link href={"/"} prefetch={false} className='flex items-center justify-center gap-2'>
             <Image src={FUUASTIconOne} alt='University Icon' width={30} height={30}/>
             <h1 className='text-lg md:text-xl tracking-tighten font-bold hidden md:block transition-all duration-500'>FUUAST</h1>
            </Link>
            <nav className='items-center gap-4 hidden md:flex'>
                <Link href={'#programs'} className='text-sm font-medium hover:underline underline-offset-4'>Program</Link>
                <Link href={'#requirements'} className='text-sm font-medium hover:underline underline-offset-4'>Requirements</Link>
                <Link href={'#contact'} className='text-sm font-medium hover:underline underline-offset-4'>Contact</Link>
                <Link target="_blank" href={"/advertisement/admsn_aut_2024.jpg"}  className='text-sm font-medium hover:underline underline-offset-4'>Advertisement</Link>
                 <Link href={AuthRoutes.SignUp} >
            <Button  size={"sm"} >Apply Now</Button>
           </Link>
            </nav>
          
           {!showMobileNav ?   <Menu className='size-6 md:hidden cursor-pointer' onClick={() => setShowMobileNav(true)} /> : <X className='size-6 md:hidden cursor-pointer' onClick={()=> setShowMobileNav(false)}/>}
        </div>
         {
          showMobileNav && <div className='md:hidden block fixed top-0 left-0 z-50 min-h-screen w-full mx-auto mt-[62.67px] bg-emerald-50'>
             <nav className='flex  md:flex flex-col  items-center gap-4 mt-5 ' onClick={()=> setShowMobileNav(false)}>
                 <Link href={AuthRoutes.SignUp} className=''>
            <Button  size={"lg"} >Apply Now</Button></Link>
                <Link href={'#programs'} className='text-sm font-medium hover:underline underline-offset-4'>Program</Link>
                <Link href={'#requirements'} className='text-sm font-medium hover:underline underline-offset-4'>Requirements</Link>
                <Link href={'#contact'} className='text-sm font-medium hover:underline underline-offset-4'>Contact</Link>
                <Link target="_blank" href={"/advertisement/admsn_aut_2024.jpg"}  className='text-sm font-medium hover:underline underline-offset-4'>Advertisement</Link>
            </nav>
           </div>
         }
    </header>
  )
}

export default Header