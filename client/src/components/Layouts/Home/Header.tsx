import { Button } from '@/components/ui/button'
import useScrollPosition from '@/hooks/useScrollYposition'
import { cn } from '@/lib/utils'
import { FUUASTIconOne } from '@/utils/fonts/font'
import { AuthRoutes } from '@/utils/routes/Routes'
import Image from 'next/image'
import Link from 'next/link'
import React, { FC } from 'react'

type Props = {}

const Header: FC<Props> = () => {
  // latest scroll position
  const scrollYPosition = useScrollPosition()
  return (
    <header className={cn(`bg-background md:py-8 md:px-6 py-6 px-4 border-b`)}>
        <div className="container flex items-center justify-between ">
            <Link href={"/"} prefetch={false} className='flex items-center justify-center gap-2'>
             <Image src={FUUASTIconOne} alt='University Icon' width={30} height={30}/>
             <h1 className='md:text-xl tracking-tighten font-semibold  text-lg transition-all duration-500'>FUUAST Islamabad</h1>
            </Link>
            <nav className='hidden md:flex items-center gap-4'>
                <Link href={'#programs'} className='text-sm font-medium hover:underline underline-offset-4'>Program</Link>
                <Link href={'#requirements'} className='text-sm font-medium hover:underline underline-offset-4'>Requirements</Link>
                <Link href={'#contact'} className='text-sm font-medium hover:underline underline-offset-4'>Contact</Link>
                <Link target="_blank" href={"/advertisement/admsn_aut_2024.jpg"}  className='text-sm font-medium hover:underline underline-offset-4'>Advertisement</Link>
            </nav>
           <Link href={AuthRoutes.Login}>
            <Button   >
                Apply Now
            </Button>
           </Link>
        </div>
    </header>
  )
}

export default Header