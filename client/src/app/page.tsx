"use client";
import Signup from "@/components/Authentication/Signup/Signup";
import { FormControl } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
export default function Home() {
  const x = Array.from(Array(100 + 1).keys()).slice(1)
  return (
    <main className="flex min-h-screen flex-col items-center justify-between p-24">
      <Select>
        <SelectTrigger>
          <SelectValue

            placeholder="Select a degree level"
          />
        </SelectTrigger>
        <SelectContent>
          {/* @ts-ignore  */}
          {x.map((id, lvl) => (
            <SelectItem key={id} value={id.toString()}>
              {lvl}
            </SelectItem>
          ))}
        </SelectContent>
      </Select>
    </main>
  );
}
