import { Role } from "../../enums/role";
import { UserMembershipModel } from "../user-membership/user-membership-model";

export interface UserProfileModel {
    id: number,
    firstName: string,
    lastName: string,
    profilePicture: string,
    address: string,
    phoneNumber: string,
    birthDate: Date | null,
    birthDateString: string,
    email: string,
    login: string,
    registrationDate: Date | null,
    registrationDateStr: string,
    role: Role,
    userMembershipModel: UserMembershipModel | null
}
