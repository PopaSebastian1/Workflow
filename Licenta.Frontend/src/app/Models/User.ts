import { Role } from './Role';
import { Skill } from './Skill';

export interface User {
  id: number;
  email: string;
  role: Role;
  firstName: string;
  lastName: string;
  profilePicture: string;
  fullName: string;
  skills: Skill[];
}
