// payload kako dolazi iz JWT-a
export interface JwtPayloadDto {
  sub: string;
  email: string;
  is_admin: string;
  is_manager: string;
  is_employee: string;
  ver: string;
  iat: number;
  exp: number;
  aud: string;
  iss: string;
}
