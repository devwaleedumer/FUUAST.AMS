import { Button } from "@/components/ui/button"
import { Heading } from "@/components/ui/heading"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { FC } from "react"

type Props = {}

const title = "Profile settings"
const description  = "Leave the password fields blank to retain old password"

const Profile : FC<Props> = ({}) => {
  return (
<>
<Heading description={description} title={title}/>
<div className=" gap-6 max-w-sm">
           <div className="mb-2">
            <Label>
                Name
            </Label>
             <Input value={"Waleed Umer"} disabled  />
           </div>
          <div className="mb-2">
            <Label>
                Email
            </Label>
             <Input value={"dev.waleedumer@gmail.com"} disabled type="email"  />
           </div>
           <form>
              <div className="mb-2">
            <Label>
              Old  Password
            </Label>
             <Input  type="password"/>
           </div>
              <div className="mb-4">
            <Label>
              New  Password
            </Label>
             <Input  type="password"/>
           </div>
            <Button className="space-y-2"  size={"sm"} >Change Password</Button>
          </form>
    </div> 
</>
    
  )
}

export default Profile