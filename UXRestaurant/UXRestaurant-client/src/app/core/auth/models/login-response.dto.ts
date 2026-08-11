export interface LoginResponseDto {
    token: string;
    refreshToken: string;
    userId: string;
    email: string;
    personName: string;
    role: string;
}