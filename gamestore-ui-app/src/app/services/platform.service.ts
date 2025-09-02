import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { filter, take, switchMap, tap, catchError } from 'rxjs/operators';
import { ApiConfigService } from './api-config.service';

export interface Platform {
  id?: string;
  type: string;
}

@Injectable({
  providedIn: 'root'
})
export class PlatformService {
  constructor(
    private http: HttpClient,
    private apiConfig: ApiConfigService
  ) {}

  getAllPlatforms(): Observable<Platform[]> {
    console.log('🎮 PlatformService.getAllPlatforms() called');
    return this.apiConfig.config$.pipe(
      filter(config => config !== null),
      take(1),
      switchMap(config => {
        const url = this.apiConfig.buildUrl(config!.platformsApiUrl);
        console.log('🌐 Fetching platforms from URL:', url);
        console.log('⚙️ Using platformsApiUrl:', config!.platformsApiUrl);
        console.log('🏠 Base URL:', config!.baseApiUrl);
        
        return this.http.get<Platform[]>(url).pipe(
          tap((response) => {
            console.log('📦 Raw platform response:', response);
          }),
          catchError((error) => {
            console.error('💥 Platform API error:', error);
            throw error;
          })
        );
      })
    );
  }

  getPlatformById(id: string): Observable<Platform> {
    const config = this.apiConfig.getConfig();
    if (!config) {
      throw new Error('API configuration not loaded');
    }

    const url = this.apiConfig.buildUrl(config.platformApiUrl, { id: id });
    return this.http.get<Platform>(url);
  }
}
