// core/company/services/company-wizard-facade.service.ts
import { inject, Injectable, signal } from '@angular/core';
import { tap } from 'rxjs';
import { CompanyApiService } from './company-api.service';
import { CompanyStateService } from './company-state.service';
import { CompanyWizardStep } from '../models/company.models';
import {
    CompleteBasicInformationRequest,
    CompleteBrandingRequest,
    CompleteContactInformationRequest,
    CompleteFiscalInformationRequest,
} from '../models/company.models';

@Injectable({ providedIn: 'root' })
export class CompanyWizardFacadeService {
    private readonly api = inject(CompanyApiService);
    private readonly state = inject(CompanyStateService);

    private readonly _currentStep = signal<CompanyWizardStep>(CompanyWizardStep.BasicInformation);
    readonly currentStep = this._currentStep.asReadonly();

    setStep(step: CompanyWizardStep): void {
        this._currentStep.set(step);
    }

    completeBasicInformation(companyId: string, slug: string) {
        const request: CompleteBasicInformationRequest = { companyId, slug };

        return this.api.completeBasicInformation(companyId, request).pipe(
            tap(() => this._currentStep.set(CompanyWizardStep.ContactInformation))
        );
    }

    completeContactInformation(companyId: string, email: string, phone: string) {
        const request: CompleteContactInformationRequest = { companyId, email, phone };

        return this.api.completeContactInformation(companyId, request).pipe(
            tap(() => this._currentStep.set(CompanyWizardStep.FiscalInformation))
        );
    }

    completeFiscalInformation(
        companyId: string,
        taxNumber: string,
        street: string,
        city: string,
        postalCode: string,
        country: string
    ) {
        const request: CompleteFiscalInformationRequest = {
            companyId, taxNumber, street, city, postalCode, country,
        };

        return this.api.completeFiscalInformation(companyId, request).pipe(
            tap(() => this._currentStep.set(CompanyWizardStep.Branding))
        );
    }

    completeBranding(companyId: string, description?: string, logoUrl?: string) {
        const request: CompleteBrandingRequest = { companyId, description, logoUrl };

        return this.api.completeBranding(companyId, request).pipe(
            tap(() => {
                this._currentStep.set(CompanyWizardStep.Completed);
                // Refresca a empresa no state global para refletir wizardStep=Completed
                this.api.getById(companyId).subscribe(company => this.state.upsertCompany(company));
            })
        );
    }

    reset(): void {
        this._currentStep.set(CompanyWizardStep.BasicInformation);
    }
}