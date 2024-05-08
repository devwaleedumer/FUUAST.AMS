import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { StoreProvider } from "@/providers/storeProvider/StoreProvider";
import { ThemeProvider } from "@/providers/themeProvider/NextThemesProvider";
import Heading from "@/components/shared/Heading";


// yes it
const inter = Inter({ subsets: ["latin"] });

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <Heading
        title="FUUAST online admission management system"
        description="FUUAST online admission management system, Student can register for any program on the fly while sitting at their home "
        keywords="fuuast,FUUAST,Fuuast,Fuuast Admissions, Admissions,online admission fuuast"
      />

      <body className={inter.className}>
        {" "}
        <StoreProvider>
          <ThemeProvider attribute="class" defaultTheme="system" enableSystem>
            {children}
          </ThemeProvider>
        </StoreProvider>
      </body>
    </html>
  );
}
