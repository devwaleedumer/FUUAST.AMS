import Link from "next/link";
import { MobileSidebar } from "./MobileSidebar";
import { cn } from "@/lib/utils";
import { Navbar } from "./Navbar";
import ThemeSwitcher from "../../shared/ThemeSwitcher";

export default function Header() {
  return (
    <div className="fixed top-0 left-0 right-0 supports-backdrop-blur:bg-background/60 border-b bg-background/95 backdrop-blur z-20">
      <nav className="h-14 flex items-center justify-between px-4">
        <div className="hidden lg:block">
        </div>
        <div className={cn("block lg:!hidden")}>
          <MobileSidebar />
        </div>  

        <div className="flex items-center gap-2">
          <Navbar />
          <ThemeSwitcher />
        </div>
      </nav>
    </div>
  );
}
