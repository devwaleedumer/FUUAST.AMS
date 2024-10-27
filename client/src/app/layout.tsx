"use client"
import { Inter } from "next/font/google";
import "./globals.css";
import { StoreProvider } from "@/providers/storeProvider/StoreProvider";
import { ThemeProvider } from "@/providers/themeProvider/NextThemesProvider";
import Heading from "@/components/shared/Heading";
import NextTopLoader from 'nextjs-toploader';
import { Toaster } from "@/components/ui/toaster";


const inter = Inter({ subsets: ["latin"] });
export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
 
  return (
    
    <html lang="en" suppressHydrationWarning>
      <Heading
        title="FUUAST online admission management system"
        description="FUUAST online admission management system, Student can register for any program on the fly while sitting at their home "
        keywords="fuuast,FUUAST,Fuuast,Fuuast Admissions, Admissions,online admission fuuast"
      />

      <body className={inter.className}>
        <NextTopLoader showSpinner={false} color={"hsl(142.1, 76.2%, 36.3%)"} />
       <StoreProvider>
        <Toaster/>
          <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
            {children}
          </ThemeProvider>
        </StoreProvider>
        
      </body>
    </html>
  );
}
