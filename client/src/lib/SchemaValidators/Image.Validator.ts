import { z } from "zod";
import { ACCEPTED_IMAGE_TYPES, MAX_FILE_SIZE } from "../configurations/UploaderConf";

export const imageUploaderSchema = z.object({
    profileImage: z
        .instanceof(File, {
            message: "Image is required"
        })
        .refine((file) => file?.size <= MAX_FILE_SIZE, `Max image size is 4MB.`)
        .refine(
            (file) => ACCEPTED_IMAGE_TYPES.includes(file?.type),
            "Only .jpg, .jpeg, .png and .webp formats are supported."
        ),
    imageRequest: z.object({
        name: z.string(),
        extension: z.string(),
        data: z.string()
    }),
})
export type ImageUploaderValues = z.infer<typeof imageUploaderSchema>;
