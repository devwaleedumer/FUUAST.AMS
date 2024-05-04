"use client";
import { useTheme } from "next-themes";
import React, { useEffect, useState } from "react";
import { BiMoon, BiSun } from "react-icons/bi";

type Props = {};

function ThemeSwitcher({}: Props) {
  const [unMounted, setMounted] = useState<boolean>();
  const { theme, setTheme } = useTheme();
  useEffect(() => setMounted(true), []);

  if (!unMounted) return null;

  return (
    <div className="flex items-center justify-center mx-4">
      {theme === "light" ? (
        <BiMoon
          className="cursor-pointer"
          fill="black"
          size={25}
          fontWeight={400}
          onClick={() => setTheme("dark")}
        />
      ) : (
        <BiSun
          className="cursor-pointer"
          size={25}
          fontWeight={400}
          fill="white"
          onClick={() => setTheme("light")}
        />
      )}
    </div>
  );
}

export default ThemeSwitcher;
