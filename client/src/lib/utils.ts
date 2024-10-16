import { type ClassValue, clsx } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function omitProps<T extends Record<string, any>, K extends keyof T>(obj: T, ...keys: K[]): Omit<T, K> {
  const result = { ...obj };
  keys.forEach((key) => {
    delete result[key];
  });
  return result;

}

export function compareJsonString(obj1: any, obj2: any) {
  return JSON.stringify(obj1) === JSON.stringify(obj2)
}