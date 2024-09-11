import React from 'react'

type Props = {}

function Requirement({}: Props) {
  return (
     <section id="requirements" className="w-full py-12 md:py-24 lg:py-32 bg-muted">
          <div className="container px-4 md:px-6">
            <div className="flex flex-col items-center justify-center text-center space-y-4">
              <div className="space-y-2">
                <div className="inline-block rounded-lg bg-white px-3 py-1 text-sm">Admission</div>
                <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">Documents Required for Admission</h2>
                <p className="max-w-[900px] text-muted-foreground md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                  Discover the steps to become a part of our vibrant university community. Learn about the admission
                  requirements and application process.
                </p>
              </div>
            </div>
            <div className="mx-auto grid max-w-5xl items-center gap-6 py-12 lg:grid-cols-2 lg:gap-12">
              {/* <img
                src="/placeholder.svg"
                width="550"
                height="310"
                alt="Requirements"
                className="mx-auto aspect-video overflow-hidden rounded-xl object-cover object-center sm:w-full lg:order-last"
              /> */}
              <div className="flex flex-col justify-center space-y-4">
                <ul className="grid gap-6">
                  <li>
                    <div className="grid gap-1">
                      <h3 className="text-xl font-bold">Academic Transcripts</h3>
                      <p className="text-muted-foreground">
                        Provide official transcripts from your educational institutions. <br />
                        Attested Copies of academic(Matric/O-Level/FA/FSC/A-Level/B.Sc/BA/MA/M.SC/MBA Marks sheets).
                      </p>
                    </div>
                  </li>
                  <li>
                    <div className="grid gap-1">
                      <h3 className="text-xl font-bold">Required documents</h3>
                      <p className="text-muted-foreground">
                        Four Passport Size Photograph.	
<br />Attested copy of CNIC/B.FORM.
                      </p>
                    </div>
                  </li>
                  <li>
                    <div className="grid gap-1">
                      <h3 className="text-xl font-bold">Standardized Test Scores</h3>
                      <p className="text-muted-foreground">Submit your NTS/GAT, or other relevant test scores Results for (MBA/MS/M.Phil/PhD).</p>
                    </div>
                  </li>
                  <li>
                    <div className="grid gap-1">
                      <h3 className="text-xl font-bold">Awaiting students</h3>
                      <p className="text-muted-foreground">
                        Hope Certificate for result awaited students
                      </p>
                    </div>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        </section>
  )
}

export default Requirement