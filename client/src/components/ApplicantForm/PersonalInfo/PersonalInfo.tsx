import { useGetApplicantPersonalInformationQuery } from '@/redux/features/applicant/applicantApi';
import React, { FC } from 'react'
import PageLoader from '../../shared/Loader';
import EditPersonalInfo from './EditPersonalInfo';
import CreatePersonalInfo from './CreatePersonalInfo';

type PersonalInfoProps = {}

const PersonalInfo: FC<PersonalInfoProps> = ({}) => {
      const { data: personalInformationData, isLoading } = useGetApplicantPersonalInformationQuery(null);
       return ( isLoading ? 
    <PageLoader/> : !personalInformationData ? <CreatePersonalInfo/> : <EditPersonalInfo personalInformationData={personalInformationData}/>
  )
}

export default PersonalInfo