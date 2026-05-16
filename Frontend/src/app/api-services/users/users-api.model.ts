// === COMMANDS (WRITE) ===

/**
 * Command for POST /register
 * Corresponds to: RegisterUserCommand.cs
 */
export interface RegisterUserCommand {
    firstName: string;
    lastName: string;
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
    phoneNumber?: string | null;
}
/**
 * Response for POST /register
 * Corresponds to: RegisterUserResultDto.cs
 */
export interface RegisterUserResultDto {

    userId: string;
    username: string;
    email: string;

}

/**
 * Command for POST /password-reset/email
 * Corresponds to: RequestPasswordResetByEmailCommand.cs
 */
export interface RequestPasswordResetByEmailCommand {
    userEmail: string;
}

/**
 * Command for POST /password-reset/security-question
 * Corresponds to: RequestPasswordResetBySecurityQuestionCommand.cs
 */
export interface RequestPasswordResetBySecurityQuestionCommand {
    userSecurityQuestionId: number;
    securityQuestionAnswer: string;
}

/**
 * Response for POST /password-reset/security-question
 * Corresponds to: PasswordResetCodeDto.cs
 */
export interface PasswordResetCodeDto {
    resetCode: string;
}


/**
 * Command for POST /password-reset/change
 * Corresponds to: PasswordResetCommand.cs
 */
export interface PasswordResetCommand{
    recoveryCode: string;
    newPassword: string;
    confirmNewPassword: string;
}

/**
 * Command for PUT /password-change
 * Corresponds to: ChangePasswordCommand.cs
 */
export interface ChangePasswordCommand {
    oldPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}

export interface ChangePhoneNumberCommand {
    phoneNumber: string;
}

export interface DeleteUserCommand {
    confirmationText: string;
}

export interface UpdateCurrentUserProfileCommand {
    username: string;
    bio?: string | null;
    countryId?: number | null;
    phoneNumber?: string | null;
}


export interface UserProfileAchievementDto {
    name: string;
    description?: string | null;
    unlockedAt?: string | null;
    imageURL?: string | null;
}


export interface UserRecentlyBoughtGameDto {
    name: string;
    coverImageURL?: string | null;
}


export interface GetUserProfileQueryDto {
    username: string;
    countryId?: number | null;
    bio?: string | null;
    profileImageURL?: string | null;
    city?: string | null;
    country?: string | null;
    email?: string | null;
    phoneNumber?: string | null;
    ownedGamesCount: number;
    isOwnProfile: boolean;
    achievements?: UserProfileAchievementDto[] | null;
    recentlyBoughtGames?: UserRecentlyBoughtGameDto[] | null;
}



// === QUERIES (READ) ===





/**
 * Response for GET /check-recovery-options
 * Corresponds to: CheckRecoveryOptionsQuery.cs
 */

export interface CheckRecoveryOptionsQuery{
    recoveryEmail: string;
}

/**
 * Response for GET /check-recovery-options
 * Corresponds to: CheckRecoveryOptionsDto.cs
 */
export interface CheckRecoveryOptionsResultDto{
    canUseSecurityQuestionRecovery: boolean;
}


/**
 * Response for GET /verify-reset-code
 * Corresponds to: VerifyResetCodeQuery.cs
 */

export interface VerifyResetCodeQuery{
    resetCode: string;
    userEmail:string;
}











