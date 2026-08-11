// core/company/models/company.models.ts

export enum BusinessType {
    Restaurant = 'Restaurant',
    Cafe = 'Cafe',
    Retail = 'Retail',
    Barbershop = 'Barbershop',
    Spa = 'Spa',
}

export enum CompanyStatus {
    PendingConfiguration = 'PendingConfiguration',
    Active = 'Active',
    Suspended = 'Suspended',
    Inactive = 'Inactive',
}

export enum CompanyWizardStep {
    BasicInformation = 'BasicInformation',
    ContactInformation = 'ContactInformation',
    FiscalInformation = 'FiscalInformation',
    Branding = 'Branding',
    Completed = 'Completed',
}

export enum CompanyRole {
    Owner = 'Owner',
    Admin = 'Admin',
    Member = 'Member',
}

export interface CompanyResponse {
    companyId: string;
    name: string;
    businessType: BusinessType;
    status: string;
    wizardStep: string;
    country: string;
    city: string;
}

export interface CompanyDashboardResponse {
    companyId: string;
    companyName: string;
    status: CompanyStatus;
    wizardStep: CompanyWizardStep;
    wizardCompleted: boolean;
}

export interface RegisterCompanyRequest {
    ownerUserId: string;
    name: string;
    businessType: BusinessType;
    country: string;
    city: string;
}

export interface RegisterCompanyResponse {
    companyId: string;
    status: string;
    wizardStep: string;
}

export interface UpdateCompanyRequest {
    companyId: string;
    name: string;
    businessType: BusinessType;
}

export interface InviteUserRequest {
    companyId: string;
    userId: string;
    role: CompanyRole;
}

export interface CompleteBasicInformationRequest {
    companyId: string;
    slug: string;
}

export interface CompleteContactInformationRequest {
    companyId: string;
    email: string;
    phone: string;
}

export interface CompleteFiscalInformationRequest {
    companyId: string;
    taxNumber: string;
    street: string;
    city: string;
    postalCode: string;
    country: string;
}

export interface CompleteBrandingRequest {
    companyId: string;
    description?: string;
    logoUrl?: string;
}